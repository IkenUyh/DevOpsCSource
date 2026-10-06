FROM eclipse-temurin:25-jdk-alpine AS builder 

WORKDIR /app 

#tải thư viện
COPY .mvn .mvn 
COPY mvnw . 
COPY pom.xml . 
RUN ./mvnw dependency:go-offline  

#build 
#COPY . .
COPY src ./src 
# /app/target/*.jar 
RUN ./mvnw package -DskipTests 

FROM eclipse-temurin:25-jre-alpine

WORKDIR /app 

COPY --from=builder /app/target/*jar app.jar 

EXPOSE 8080 

CMD ["java", "-jar", "app.jar"]

# docker build -t java_be_image .
# docker run --name java_be_container -d -p 8003:8080 --env-file=.env java_be_image 
