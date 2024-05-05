#!/usr/bin/python3
# Code for generating the salt and hashed password for saving in authorization_module.py
# on the AWS Lambda server.
#
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

# NOTE: This must be run on a *nix (including WSL)

from argparse import ArgumentParser
from authorization_module import authorize_user_from_dict
import sys
import crypt
import pdb

def main(argv):
   # Define and parse the command line arguments
   cli = ArgumentParser(description = "Create a new authrorization_module entry")
   cli.add_argument("-e","--email", type=str, required=True, dest="email",
                    help='email used for the Amazon Polly usage authorization')
   cli.add_argument("-p","--password", type=str, required=True, dest="password",
                    help='password used for Amazon Polly usage authorization')
   arguments = cli.parse_args()
   
   password = bytes(arguments.password, 'utf-8')

   salt = crypt.mksalt(method=crypt.METHOD_SHA512)
   hashed = crypt.crypt(arguments.password,salt=salt)
   auth_entry = {arguments.email:{
      'salt':salt,
      'password':hashed
      }}
   
   print (auth_entry)

    
# end def main(argv)

# Call the main function.
if __name__ == "__main__":
   main(sys.argv[1:])

    
