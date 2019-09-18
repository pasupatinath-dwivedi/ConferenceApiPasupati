# ConferenceApi
Service is built on .Net Core 2.1. 
1.	.Net Core 2.1 Api it connects through service and calls the Azure Api
2.	To run the Api,new instance can be run or F5 will run the api, it will run on port 7000 and https on 7001,
3.	Api controller test cases are written using XUnit
4.  Basic Authentication is used using custom implementation of AuthenticationHandler
5. Global Exception Handler is written using Extension method on IApplicationBuilder

## Swagger 

.Net core api is configured with swagger URL 
Run the instance of Training Api and use below link for swagger

http://locahost:7000/swagger


## List of service api

Action | Method | Route
------------ | ------------- |--------
speakersandsessions	|GET request. Returns success message and statuscode and json object array.	| api/v1/conference/speakersandsessions
GetAsync |GET request. Returns success message and statuscode and json object of session detail.| api/v1/conference/session/{id}
