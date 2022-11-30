# HomeStream
A self hosted video streaming site built with angular and golang.

# Setting up local MongoDB
## Installing mongodb
## Enabling authentication
1. Start mongodb deamon
```
$ sudo systemctl start mongod
```
2. Connect to mongodb using shell
```
$ mongosh
```
3. Use admin database
```
$ use admin
``` 
4. Create new user. Replace USER_NAME & USER_PASSWORD with the values that you want.
```
$ db.createUser(
... {
... user: "USER_NAME",
... pwd: "USER_PASSWORD",
... roles: ["readWriteAnyDatabase"]
... }
... )
```
5. Exit shell
```
$ exit
```
7. Modify the configuration file to enable authentication
```
$ sudo nano /etc/mongod.conf
```
8. Uncomment security option and add authentication. The previous content will be 
```
#secutiry
```
9. After updating configuration it will look like this. Three are 2 spaces before the `authorization: enabled` line. Save using `Ctrl+X`, confirm using `Y` and `Enter`.
```
security:
  authorization: enabled
``` 
10. Restart mongod
```
$ sudo systemctl restart mongod
```
11. Verify the deamon is running and status is active
```
$ sudo systemctl status mongod
```
12. Connect to mongoDb using shell.
```
$ mongosh -u "USER_NAME" -p "USER_PASSWORD" --authenticationDatabase "admin"
```

If everything is correct it will connect to the default test database. To verify whether authentication is working use `$ exit` command and try to connect to mongoDb using 
`$ mongosh` and list databases `$ show dbs`. It will return an error saying "MongoServerError: command listDatabases requires authentication".
