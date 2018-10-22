# TitanSoft Movie API


This project consists of several files which make up the system.  The most notable files will be within the 
project titled TitanSoft.Api - these files are called:

1. Controllers/MembersController.cs
2. Controllers/MoviesController.cs
3. Controllers/RentalsController.cs

All of these files are fairly self-explanatory and make up all of the accessible endpoints for the Movie API.
There is a Swagger endpoint as well.  To make things as simple as possible the swagger endpoint is accessible
at the web root ( "/" ). Generally, this can be accessed when running locally by visiting the following URL
in your browser:

http://localhost:5001/

The different endpoints can be manipulated using this interface depending on the data submitted with the HTTP
request. Some of the endpoints are in fact protected using a JWT and an authentication mechanism. Most of the
MovieController action methods allow for anonymous access. The Swagger endpoint will demonstrate how these 
endpoints can be used. The main solution also holds a Postman (http://getpostman.com) export that an be opened
and run from within the Postman app.

The authentication endpoing is located at:

https://localhost:5001/auth/

With the following JSON object:

{
   "username": "test@gmail.com",
   "password": "test"
 }


There is also an embedded open-source database (RavenDB) system that is supposed to be only accessible by 
authenticated users, however, that restriction has been relaxed when visiting from the localhost URl.  
The RavenDB admin portal can be accessed at the following URL:

http://localhost:60956/

Once there the Movie database can be accessed by visiting the 'Databases' link on the left hand side. Next,
click on the 'TitanSoftStore' dateabase store.  From here you will see several collections that an be managed
from the RavenDB admin portal.

 
 
