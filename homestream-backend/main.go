package main

import (
	"homestream-api/common/constants"
	"homestream-api/common/helper"
	"homestream-api/controller"
	"homestream-api/helper/dbImpl"
	"log"

	"github.com/gin-gonic/gin"
	"github.com/joho/godotenv"
)

func init() {
	err := godotenv.Load(".env")
	if err != nil {
		log.Println("Failed to load env file")
	}
}

func main() {
	r := gin.New()
	r.Use((gin.Logger()))
	r.Use((gin.Recovery()))

	fileRouteGroup := r.Group("/file")
	{
		fileRouteGroup.GET("", controller.GetAllDirectories)
		fileRouteGroup.DELETE("", controller.DeleteAllDirectories)
		fileRouteGroup.PUT("", controller.AddDirectory)
		fileRouteGroup.DELETE("/:id", controller.DeleteDirectory)
	}

	movieRouteGroup := r.Group("/movie")
	{
		movieRouteGroup.GET("", controller.GetMovies)
		movieRouteGroup.PUT("", controller.AddMovies)
		movieRouteGroup.GET("/:id", controller.StreamMovie)
		movieRouteGroup.POST("/:id", controller.UpdateMovie)
		movieRouteGroup.DELETE("/:id", controller.DeleteMovie)
	}

	mDb := dbImpl.GetMongoDbService()
	defer mDb.Close()

	mDb.InitializeDatabasesAndCollections()

	r.Run(":" + helper.GetStringConfiguration(constants.KEY_PORT, constants.DEFAULT_PORT))
}
