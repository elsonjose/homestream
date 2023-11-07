package handler

import (
	"homestream-api/repository"
	"homestream-api/model"
	"net/http"

	"github.com/gin-gonic/gin"
	"go.mongodb.org/mongo-driver/bson"
)

func GetAllDirectories(c *gin.Context) {

	mDb := repository.GetMongoDbService()
	defer mDb.Close()

	directoryEntities, err := mDb.GetDirectoriesCollection().Find(mDb.Context, bson.D{{}})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{
			"message": "Failed to fetch directories",
		})
	}

	var directories []model.Directory = []model.Directory{}
	for directoryEntities.Next(mDb.Context) {
		var directory model.Directory
		directoryEntities.Decode(&directory)
		directories = append(directories, directory)
	}

	c.JSON(http.StatusOK, gin.H{
		"data": directories,
	})
}
