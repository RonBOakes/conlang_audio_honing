##### NOTE: This python source is placed in an Amazon Lambda instance and
#####       cannot be run stand alone
# Amazon Web Services Lambda and Restful Code for Conlang Audio Honing
# Copyright (C) 2023-2024 Ronald B. Oakes
#
# This program is free software: you can redistribute it and/or modify it under
# the terms of the GNU General Public License as published by the Free Software
# Foundation, either version 3 of the License, or (at your option) any later
# version.
#
# This program is distributed in the hope that it will be useful, but WITHOUT
# ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS
# FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along with
# this program.  If not, see <http://www.gnu.org/licenses/>.
import json
import boto3
import base64
import logging
import time
from contextlib import closing
from botocore.exceptions import BotoCoreError, ClientError

polly_client = boto3.client("polly")
s3_client = boto3.client("s3")
logger = logging.getLogger()
s3bucket = 'lambdaspeach'

def create_bucket_if_needed(bucket_name):
    response = s3_client.list_buckets()
    
    for bucket in response['Buckets']:
        if bucket["Name"] == bucket_name:
            return

    respone = s3_client.create_bucket(
        Bucket=bucket_name,
    )
    logger.debug("Created Bucket")
#end create_bucket_if_needed

def start_synthisis(ssml,voice,filetype):
    logger.info("Generating text for the " + voice + " voice")
    
    s3bucket = 'lambdaspeach'
    
    if filetype == 'ogg':
        filetype = 'ogg_vorbis'
    elif filetype != 'mp3':
        filetype = 'ogg_vorbis'
    
    create_bucket_if_needed(s3bucket)
    
    response = polly_client.start_speech_synthesis_task(VoiceId=voice,
                OutputS3BucketName=s3bucket,
                OutputFormat=filetype,
                TextType='ssml',
                Text=ssml,
                Engine='neural')
    logger.debug("Response from start_speech_synthesis_task",extra={'response':str(response)})
    taskId = response['SynthesisTask']['TaskId']

    task_status = polly_client.get_speech_synthesis_task(TaskId = taskId)
    uri = task_status['SynthesisTask']['OutputUri']
    logger.debug("Polly task URI:",extra={'uri':uri})
    logger.info("Sending task URI back to caller")
    return_dict = {
        'task_id':taskId,
        'uri':uri
    }
    body = json.dumps(return_dict)
    logger.info("Sending start results",extra={'response':body})
    return {
        'statusCode': 200,
        'headers':'text/json',
        'isBase64Encoded': False,
        'body': body
    }

#end start_synthisis

def get_voices():
    voice_struct_data = polly_client.describe_voices(Engine='neural')
    logger.debug('voice_struct',extra={'voice_struct':str(voice_struct_data))})
    body = json.dumps(voice_struct_data)
    return{
        'statusCode': 200,
        'headers':'text/json',
        'isBase64Encoded': False,
        'body': body
    }

def status_task(task_id):
    task_status = polly_client.get_speech_synthesis_task(TaskId = task_id)
    logger.debug('task_status',extra={'task_status':str(task_status)})
    if task_status['SynthesisTask']['TaskStatus'] == 'completed':
        logger.info("Sending Task Complete")
        return {
            'statusCode': 200,
            'isBase64Encoded': False,
            'body': 'COMPLETE',
        }
    elif task_status['SynthesisTask']['TaskStatus'] == 'failed':
        logger.info("Sending Task Failed")
        return {
            'statusCode': 200,
            'isBase64Encoded': False,
            'body': 'FAILED',
        }
    else:
        logger.info("Sending Task Running")
        return {
            'statusCode': 200,
            'isBase64Encoded': False,
            'body': 'RUNNING',
        }

#end status_task

def download_file(uri,filetype):
    if filetype == 'ogg':
        filetype = 'ogg_vorbis'
    elif filetype != 'mp3':
        filetype = 'ogg_vorbis'
    
    response = s3_client.list_objects_v2(Bucket=s3bucket)
    
    s3file_list = response['Contents']
    for s3file in s3file_list:
        if s3file['Key'] in uri:
            s3audiofile = s3file
            break
    
    if s3audiofile:
        response = s3_client.get_object(
            Bucket=s3bucket,
            Key=s3file['Key']
        )
        
    # Access the audio stream from the response
    if "Body" in response:
        # Note: Closing the stream is important because the service throttles on the
        # number of parallel connections. Here we are using contextlib.closing to
        # ensure the close method of the stream object will be called automatically
        # at the end of the with statement's scope.
        with closing(response['Body']) as stream:
            try:
                if filetype == 'ogg_vorbis':
                    headers = '{ "Content-Type": "audio/ogg" }'
                else:
                    headers = '{ "Content-Type": "audio/mp3" }'
                audio = stream.read()
                logger.info("Sending stream back to the caller")
                return {
                    'statusCode': 200,
                    'headers':headers,
                    'isBase64Encoded': True,
                    'body': base64.b64encode(audio),
                }
            except IOError as error:
                logger.error("Exception reading the audio stream from Polly:",extra={'error':error})
                return {
                    'statusCode': 500,
                    'errorMessage': error
                    }
    else:
        logger.error("No audio generated")
        return {
                'statusCode': 500,
                'errorMessage': 'No Audio Generated'
                }

#end download_file

def delete_file(uri):
    response = s3_client.list_objects_v2(Bucket=s3bucket)
    
    s3file_list = response['Contents']
    if uri != '':
        for s3file in s3file_list:
            if s3file['Key'] in uri:
                s3_client.delete_object(Bucket=s3bucket,Key=s3file['Key'])
                break
    else:
        for s3file in s3file_list:
            s3_client.delete_object(Bucket=s3bucket,Key=s3file['Key'])
    logger.info("Sending delete complete")
    return {
        'statusCode': 200,
        'isBase64Encoded': False,
        'body': 'COMPLETE',
    }

#end delete_file

def lambda_handler(event, context):

    logger.debug("**** event received",extra={'event':event})
    logger.debug("**** context",extra={'context':str(context)})

    # Verify that we are getting the right type of event
    if 'headers' not in event:
        logger.error("Invalid event received - no headers")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - no headers'
        }
    if 'requestContext' not in event:
        logger.error("Invalid event received - no requestContext")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - no requestContext'
        }
    if 'content-type' not in event['headers']:
        logger.error("Invalid event received - no content-type")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - no content-type'
        }
    if 'json' not in event['headers']['content-type']:
        logger.error("Invalid event received - not json content")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - not json content'
        }
    if 'http' not in event['requestContext']:
        logger.error("Invalid event received - not http origin")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - not http origin'
        }
    if 'method' not in event['requestContext']['http']:
        logger.error("Invalid event received - no method in http")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - no method in http'
        }
    if 'POST' not in event['requestContext']['http']['method']:
        logger.error("Invalid event received - not POST")
        return {
        'statusCode': 500,
        'errorMessage': 'Invalid event received - not POST'
        }

    post_data = json.loads(event['body'])
    logger.debug("**** post_data received",extra={'post_data':str(post_data)})
    

    if 'start' in post_data and 'ssml' in post_data and 'voice' in post_data:
        ssml = post_data['ssml']
        voice = post_data['voice']
        if 'filetype' in post_data:
            filetype = post_data['filetype']
        else:
            filetype = 'ogg'
        return start_synthisis(ssml,voice,filetype)
    elif 'taskStatus' in post_data and 'task_id' in post_data:
        task_id = post_data['task_id']
        return status_task(task_id)
    elif 'downloadFile' in post_data and 'uri' in post_data:
        uri = post_data['uri']
        if 'filetype' in post_data:
            filetype = post_data['filetype']
        else:
            filetype = 'ogg'
        return download_file(uri,filetype)
    elif 'delete' in post_data:
        if 'uri' in post_data:
            uri = post_data['uri']
        else:
            uri = ''
        delete_file(uri)
    elif 'systemStatus' in post_data:
        return {
            'statusCode':200,
            'body':"OK"
        }
    elif 'requestVoices' in post_data:
        return get_voices()
    else:
        logger.error("Invalid post_data received:",extra={'post_data':post_data})
        return {
            'statusCode': 500,
            'errorMessage': "Invalid post_data recived"
            }
        
    
