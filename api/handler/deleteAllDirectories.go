package handler

import (
	"homestream-api/repository"
	"net/http"

	"github.com/gin-gonic/gin"
	"go.mongodb.org/mongo-driver/bson"
)

func DeleteAllDirectories(c *gin.Context) {

	mDb := repository.GetMongoDbService()
	defer mDb.Close()

	_, err := mDb.GetDirectoriesCollection().DeleteMany(mDb.Context, bson.D{{}})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{
			"message": "Failed to delete directories",
			"data":    false,
		})
	}

	c.JSON(http.StatusOK, gin.H{
		"data": true,
	})
}