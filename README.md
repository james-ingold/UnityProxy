A proxy using ASP.NET Web API for accessing Unity web services.

When trying to access the Unity API from JavaScript, one will likely get a Cross-Origin Resource Sharing-related exception raised by the browser, along the lines of

"Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin '*******' is therefore not allowed access. The response  had HTTP status code 405."
This error refers to the much more general issue of Cross-Origin Resource Sharing (CORS), and the fact that the Unity API doesn't support it.