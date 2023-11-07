package helper

import (
	"os"
	"strconv"
)

func GetStringConfiguration(key string, defaultValue string) string {
	value := os.Getenv(key)
	if len(value) == 0 {
		return defaultValue
	}
	return value
}

func GetIntConfiguration(key string, defaultValue int) int {
	value := os.Getenv(key)
	convertedValue, err := strconv.Atoi(value)
	if err != nil {
		return defaultValue
	}
	return convertedValue
}
