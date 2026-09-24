-- Create a contained database user for development using the demo credentials.
-- Run this script in the target database (Azure Data Studio / Query Editor).
-- NOTE: replace <your-db> server details where appropriate when you run the connection string.
CREATE USER [employee_demo] WITH PASSWORD = 'DemoPass123!';

-- Grant minimum privileges for app operations in the prototype.
ALTER ROLE db_datareader ADD MEMBER [employee_demo];
ALTER ROLE db_datawriter ADD MEMBER [employee_demo];

-- Optional (only if you plan to run EF migrations from the app):
-- ALTER ROLE db_ddladmin ADD MEMBER [employee_demo];