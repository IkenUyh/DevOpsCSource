package main

import (
	"database/sql"
	"log"

	"github.com/gin-gonic/gin"
	_ "github.com/lib/pq"

	"os"

	"github.com/joho/godotenv"
)

func main() {
	godotenv.Load()

	DATABASE_URL := os.Getenv("DATABASE_URL")
	log.Println("DATABASE_URL=", DATABASE_URL)
	log.Println("ANH LONG")
	db, _ := sql.Open("postgres", DATABASE_URL)

	router := gin.Default()
	router.Use(func(c *gin.Context) {
		c.Header("Access-Control-Allow-Origin", "*")
		c.Header("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS")
		c.Header("Access-Control-Allow-Headers", "*")

		if c.Request.Method == "OPTIONS" {
			c.AbortWithStatus(204)
			return
		}

		c.Next()
	})
	router.GET("/", func(c *gin.Context) {
		c.JSON(200, gin.H{
			"message": "Hello from Go Gin Web Framework!!!",
		})
	})
	router.GET("/user", func(c *gin.Context) {
		rows, err := db.Query("SELECT name FROM users")

		if err != nil {
			c.JSON(500, gin.H{"error": err.Error()})
			return
		}
		defer rows.Close()

		users := make([]string, 0)

		for rows.Next() {
			var name string
			rows.Scan(&name)
			users = append(users, name)
		}

		c.JSON(200, gin.H{
			"source_code": "go",
			"users":       users,
		})
	})
	router.Run() // listens on 0.0.0.0:8080 by default
}
