package repository

import (
	"context"
	"homestream-api/constants"
	"homestream-api/helper"
	"log"
	"os"
	"time"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"golang.org/x/exp/slices"
)

type MongoDbHelper struct {
	ConnectionString string
	Client           *mongo.Client
	Context          context.Context
	ContextCancel    context.CancelFunc
}

func GetMongoDbService() *MongoDbHelper {
	mongoDbHelper := InitializeDatabase()
	return mongoDbHelper
}

func (mDb *MongoDbHelper) Close() {
	defer func() {
		if err := mDb.Client.Disconnect(mDb.Context); err != nil {
			panic(err)
		}
	}()
}

func InitializeDatabase() *MongoDbHelper {
	databaseTimeout := helper.GetIntConfiguration(constants.DATABASE_TIMEOUT, constants.DEFAULT_DATABASE_TIMEOUT)
	mDbHelper := new(MongoDbHelper)
	mDbHelper.Context, mDbHelper.ContextCancel = context.WithTimeout(context.Background(), time.Duration(databaseTimeout)*time.Second)
	mDbHelper.ConnectionString = os.Getenv(constants.MONGO_DB_CONNECTION_STRING)
	client, err := mongo.Connect(mDbHelper.Context, options.Client().ApplyURI(mDbHelper.ConnectionString))
	if err != nil {
		log.Fatal("Failed to connect to mongodb")
		log.Println(err)
	}
	mDbHelper.Client = client
	return mDbHelper
}

func (mDbHelper *MongoDbHelper) InitializeDatabasesAndCollections() {

	dBNames, err := mDbHelper.Client.ListDatabaseNames(mDbHelper.Context, bson.M{})
	if err != nil {
		log.Fatal(err)
	}

	isHomeStreamDatabaseCreated := slices.Contains(dBNames, constants.DB)

	if !isHomeStreamDatabaseCreated {
		log.Println("Creating required collections")
		mDbHelper.Client.Database(constants.DB).CreateCollection(mDbHelper.Context, constants.MOVIES)
		mDbHelper.Client.Database(constants.DB).CreateCollection(mDbHelper.Context, constants.DIRECTORIES)
	}
}

func (mDbHelper *MongoDbHelper) GetDirectoriesCollection() *mongo.Collection {
	return mDbHelper.Client.Database(constants.DB).Collection(constants.DIRECTORIES)
}

func (mDbHelper *MongoDbHelper) GetMoviesCollection() *mongo.Collection {
	return mDbHelper.Client.Database(constants.DB).Collection(constants.MOVIES)
}
