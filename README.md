# TitanSoft Movie API


This project consists of several files which make up the system.  The most notable files will be within the 
project titled TitanSoft.Api - these files are called:

1. Controllers/MembersController.cs
2. Controllers/MoviesController.cs
3. Controllers/RentalsController.cs

All of these files are fairly self-explanatory and make up all of the accessible endpoints for the Movie API.
There is a Swagger endpoint as well.  To make things as simple as possible the swagger endpoint is accessible
at the web root ( "http://localhost:5001/" ). Generally, this can be accessed when running locally by visiting the following URL
in your browser:

http://localhost:5001/

The different endpoints can be manipulated using this interface depending on the data submitted with the HTTP
request. Some of the endpoints are in fact protected using a JWT and an authentication mechanism. Most of the
MovieController action methods allow for anonymous access. The Swagger endpoint will demonstrate how these 
endpoints can be used. The main solution also holds a Postman (http://getpostman.com) export that an be opened
and run from within the Postman app.

The protected APIs can be used by authenticating against the following URL:

https://localhost:5001/api/v1/members/auth

with a POST HTTP request with the following JSON body:

{
    "username": "test@gmail.com",
    "password": "test"
}

This API endpoint will return a JWT for submission to the protected API endpoints via the following authorization 
header:

Authorization Bearer xxxxxxxxxxx


The API makes use of an embedded open-source database (RavenDB) that is supposed to be only accessible by 
authenticated users, however, that restriction has been relaxed when visiting from the localhost URl.  
The RavenDB admin portal can be accessed at the following URL:

http://localhost:60956/

Once there the Movie database can be accessed by visiting the 'Databases' link on the left hand side. Next,
click on the 'TitanSoftStore' dateabase store.  From here you will see several collections that an be managed
from the RavenDB admin portal.

 
 -----
 Talk about why cmomand / queyr pattern was used vs repository even though they both lend to seperation of concerns by reducing controller bloat 
 Talk about why command / query objs were intantiated directly in service layer and also note they can be injected
 Talk about project layer structure and why
 Talk about why DI was used (open to change down the road etc)
 Discuss why the MS ILogger was used vs Serilogs (gives us some degree of change in the future)
