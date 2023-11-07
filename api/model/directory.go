package model

import "go.mongodb.org/mongo-driver/bson/primitive"

type Directory struct {
	Id            primitive.ObjectID `json:"id" bson:"_id"`
	DirectoryPath string             `json:"path" binding:"required" bson:"path"`
}
