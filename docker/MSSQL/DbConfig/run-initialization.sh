# Wait to be sure that SQL Server came up
sleep 20s

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the Dockerfile
echo "turn on run-init.sh"
echo "attach database Start >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> "
/opt/mssql-tools/bin/sqlcmd -S "mssql-server" -U sa -P "123@123a" \
-Q "CREATE DATABASE [techtechcare_eShopSolution] ON (FILENAME = '/var/opt/mssql/data/techtechcare_eShopSolution.mdf'),(FILENAME = '/var/opt/mssql/data/techtechcare_eShopSolution_log.ldf') FOR ATTACH"
echo "attach database End >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> "