# IMPORTANT

Currently, we are doing manual migrations. I am having issues with EntityFramework directly applying migrations to the Supabase PostgreSQL db.
To run a manual migration:
1. Finish editing C# models
2. Open Package Manager Console
3. Set 'Default Project' to 'StraySafe.Data' # VERY IMPORTANT!!!
4. Write 'Add-Migration [WhatYouAdded]', example 'Add-Migration AddedUserSubmissionCount' and execute
5. This will create a migration file that you can commit
6. Next write 'Script-Migration', this will generate a SQL script that will be applied to the DB
7. Save the SQL script in this ManualMigrations folder for record keeping with the name as the date (ex: '4_6_2025' for April 6th, 2025)
8. Copy the SQL script and paste it into the Supabase SQL Editor. Execute and save the query with the same naming convention (ex: 4_6_2025)
9. Database is now updated!