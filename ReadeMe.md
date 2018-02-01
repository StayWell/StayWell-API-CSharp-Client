# StayWell API for .Net and Javascript Clients

The most current documentation of the is located at [https://admin.kramesstaywell.com/] {https://admin.kramesstaywell.com/docs#articles/home-page}

## About StayWell IQ API

The StayWell IQ API is the primary way to interact with StayWell's products, services, and content in a programmatic way. It enables you to search for, retrieve, and render the StayWell content quickly and easily. It also provides a way to interact with our many other products and services enabling you to integrate, extend, and build your own solutions to help your users live a healthier life and StayWell.

## What is in this library

We have included all of the functionality required to manage and retrieve content from the StayWell API. 

## Examples

There are two examples included.

First is a very simple console application that demonstrates how to connect to the API and retrieve content.

Second is a ASP.NET MVC web application that demonstrates one way to display and retrieve content.

The ASP.NET Website contains a section of HowTo examples that demonstrate work with each type of object in the API.

## Best Practice Recommendations

You will notice that we do not make API calls directly to the StayWell API from client side JavaScript code. While this is possible you will need to find a way to ensure that your Secret Key is safe and not exposed in client code. 
We also implemented a very simple contrived encryption class to encrypt the Secret Key in the configuration files. Do not use this example in your implementation. Work with your security team to determine how to secure the Secret Key. **Do not follow our simple contrived encryption example**

## License
This project is licensed under the MIT License - see the [License]{LICENSE.txt}




