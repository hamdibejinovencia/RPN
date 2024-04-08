Some tasks should be performed :
- Improving the exceptions management to be more precise when the user types wrong input. Actually, the http request returns 500 status code.
- Improving the caching by may be using redis instead of InMemoryCache to assure persistency of data
- Adding some properties to track creation and modification dates and may be the user performing the action
- Adding connection to real database and store in it all data
- Adding another operator which is % (modulo)
- Adding some loggers in all layers and log events
- Deal with the issue of slashes in the param, a catch-all route parameter would need to be used to grab everything after the route template.