package controller

import (
	handler "homestream-api/handler"

	"github.com/gin-gonic/gin"
)

func GetAllDirectories(c *gin.Context) {
	handler.GetAllDirectories(c)
}

func DeleteAllDirectories(c *gin.Context) {
	handler.DeleteAllDirectories(c)
}

func AddDirectory(c *gin.Context) {
	handler.AddDirectory(c)
}

func DeleteDirectory(c *gin.Context) {

}
