package handler

import (
	"homestream-api/repository"
	"homestream-api/model"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
	"go.mongodb.org/mongo-driver/bson"
)

func AddDirectory(c *gin.Context) {

	var record model.Directory
	var err = c.BindJSON(&record)
	if err != nil {
		log.Println(err)
		c.JSON(http.StatusBadRequest, gin.H{
			"message": "Invalid payload",
		})
		return
	}

	mDb := repository.GetMongoDbService()
	defer mDb.Close()

	findFilter := bson.M{
		"path": record.DirectoryPath,
	}

	existingDirectory := mDb.GetDirectoriesCollection().FindOne(mDb.Context, findFilter)
	var existingDirectoryEntity model.Directory
	err = existingDirectory.Decode(&existingDirectoryEntity)
	if err != nil && existingDirectory.Err().Error() == "" {
		c.JSON(http.StatusInternalServerError, gin.H{
			"message": "Failed to decode existing directory",
		})
		return
	}

	if existingDirectoryEntity.DirectoryPath == record.DirectoryPath {
		c.JSON(http.StatusInternalServerError, gin.H{
			"message": "A directory with the same path already exists",
		})
		return
	}

	insertedDirectory, err := mDb.GetDirectoriesCollection().InsertOne(mDb.Context, bson.M{"path": record.DirectoryPath})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{
			"message": "Failed to add directory",
			"data":    false,
		})
		return
	}

	c.JSON(http.StatusOK, gin.H{
		"data": insertedDirectory.InsertedID,
	})
}
