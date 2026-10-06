FROM golang:1.26.2-alpine AS builder

WORKDIR /app

# tải thư viện
COPY go.mod .
COPY go.sum . 
RUN go mod download 

# build 
COPY . .

RUN CGO_ENABLED=0 GOOS=linux go build -o go_gin_be .

FROM alpine:latest  

WORKDIR /app

COPY --from=builder /app/go_gin_be .

EXPOSE 8080 

CMD ["./go_gin_be"]

# docker build -t go_gin_be_image .
# docker run --name go_gin_be_container -d -p 8002:8080 --env-file=.env go_gin_be_image