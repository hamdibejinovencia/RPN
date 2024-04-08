Some tasks should be performed :
- Improving the exceptions management to be more precise when the user types wrong input. Actually, the http request returns 500 status code.
- Improving the caching by may be using redis instead of InMemoryCache to assure persistency of data
- Adding some properties to track creation and modification dates and may be the user performing the action
- Adding some loggers in all layers and log events
- Thinking may be about implementing an UI (React, Angular) to manage RPN functionalities by consuming this web app
- Adding Api Key to authorize access to APIs in order to secure that app
- When the data grows up, we have to think about adding some pagination to chunk data when retrieving it so we can use such as limit, offset, isLastPage properties to paginate data and avoid crashing because of StackOverFlow.
- Improve performances by using the GZIP compression algorithm. GZIP is a widely used compression method that reduces the size of HTTP responses before they are sent over the network.

- ✅Deal with the issue of slashes in the param, a catch-all route parameter would need to be used to grab everything after the route template.
This can be done if we have the op param at the end of the route but in our case, it exists in the middle.
The solution I have found temporarly that I have assigned the ':' symbol to the division operation

- ✅ Another issue has been found and resolved when I have deployed the app on Azure : https://rpn-kata-hbeji.azurewebsites.net/swagger/index.html
The problem was with the POST /api/v1/rpn/op/{op}/stack/{stackId} endpoint when using the + operator
The code was working on my machine because I was using kestrel. Running the same code with IIS in Azure didn't work with this error:
  The resource you are looking for has been removed, had its name changed, or is temporarily unavailable.
To solve this, I had to add a web.config file with the allowDoubleEscaping="true" inside