Tasks that can described in the backlog :
- Implementing a WebApi application Tool to write in Reverse Polish Notation RPN: a post-fixed notation which allows to write mathematical expression without parenthesis.
- The business logic rules must be respected as explained in the PDF file
- Some improvements could be done to take a whole expression as parameter and do operations on it
- This expression which is string must be checked before performing actions on it
- Once checked, this expression must be converted to post-fixed representation.
Example : ( 5 + 2 ) * 3 => 5 2 + 3 *
- Once converted, the user can use the same endpoints already implemented to evaluate that expression and return the final result
- you can use this URL to focus on these post-fixed expressions : https://scanftree.com/Data_Structure/prefix-postfix-infix-online-converter

Some other perspectives that could be refined in this same context :
- Adding connection to real database and store in it all data :
       - choosing the appropriate ORM : EF, Dapper
       - choosing the Database Approach or the CodeFirst Approach
- Adding another operator which is % (modulo)
- Separating environments when deploying and set configs for each env : dev, integration, staging, production
- Planning some features to create pipelines for CI/CD (Docker file, yml files for resources, deployment, pods, etc)
