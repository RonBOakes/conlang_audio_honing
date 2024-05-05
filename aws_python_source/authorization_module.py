##### NOTE: This python source is placed in an Amazon Lambda instance and
#####       cannot be run stand alone
# Amazon Web Services Dummy Authorization Code
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

from ast import Compare
import crypt
from hmac import compare_digest as compare_hash
import logging

logger = logging.getLogger()

# In a working version, check the tokens against valid tokens, and return true if the token is
# currently valid.
def authorize_user(auth_email,auth_passwd):
    # Authoization dictionary will map user's email to a dictionary that will contain a salt and 
    # a hashed password.  As saved in github, this will just have the empty version.
    authorization_dict = {
        'dummy@dummy': {
            'salt':'',
            'password':''
        }
    }
    
    return authorize_user_from_dict(auth_email,auth_passwd,authorization_dict)


def authorize_user_from_dict(auth_email,auth_passwd,authorization_dict):
    if auth_email not in authorization_dict:
        return False
    

    salt = authorization_dict[auth_email]['salt']
    logger.debug("About to authorize",extra={'email':auth_email,'salt':salt})
    hash = crypt.crypt(auth_passwd, salt)
    logger.debug("Authorizing user",extra={'email':auth_email,'salt':salt,'hash':hash,'db_hash':authorization_dict[auth_email]['password']})
    if compare_hash(hash, authorization_dict[auth_email]['password']):
        return True
    else:
        return False
    

