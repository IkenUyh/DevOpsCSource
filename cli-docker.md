# IMAGE
Tạo image: docker build -t my-app-image .
# Tạo ra file code, khi đã được chốt (đã gọn ràng mã hóa)-> file cuối cùng

Show danh sách image/container: docker image/container ls/list 

Xóa image: docker (image remove)/rmi [IMAGE_NAME/ID]

Đổi tên image: docker image tag [old_name] [new_name]

Đưa images lên docker hub: docker push [name:tag/ID]

Đóng gói Docker Image thành một file: docker save -o [đường_dẫn/tên_file.tar] [IMAGE_NAME/ID]

Import từ docker hub: docker pull [name:tag/ID]

Import từ file: docker load -i [đường_dẫn/tên_file.tar]

# CONTAINER 
Tạo container: docker run --name my-app-container -d -p 3000:3000 my-app-image
-d: detached mode: chế độ nền, không chiếm terminal hiện tại
-p: publish: ánh xạ port của máy host với port của container

Show danh sách container: docker ps = process status

Bật tắt container: docker (container) start/stop [CONTAINER_NAME/ID]

# Tab logs docker
docker container logs [CONTAINER_NAME/ID]
docker logs [CONTAINER_NAME/ID]

# Tab inspect docker
docker inspect [CONTAINER_NAME/ID]

# Tab stats docker
docker stats [CONTAINER_NAME/ID]

# DOCKER OTHER 
docker login
# Cần GUI

docker login -u <username>
# Cần CLI là đủ 

# NETWORK bên ngoài là public trong private 
- bridge:
    - Network mặc định cho container (không gắn network gì thì tự động gắn)
    - Mỗi container sẽ có IP riêng
    - IP <=> CONTAINER_NAME (giống domain)
- host: 
    - Dùng IP của sever 
    - Port dễ bị trùng 
- none: 
    - Container bị cô lập => tăng bảo mật 
    - job / bot 

docker network ls
docker network create [NETWORK_NAME]
docker network remove [NETWORK_NAME/ID]

gắn lúc container đã tồn tại: docker network connect [NETWORK_NAME] [CONTAINER_NAME/ID]
gắn lúc container chưa tồn tại (chưa chạy lệnh run): docker run --network [NETWORK_NAME] --name [CONTAINER_NAME] -d -p port:port [IMAGE_NAME]
docker network disconnect [NETWORK_NAME] [CONTAINER_NAME/ID]
docker inspect [NETWORK_NAME/ID]

# VOLUME 
Xóa container bằng giao diện thì sẽ bị mất volume 
Còn xóa bằng CLI thì sẽ không mất
Mà mount = volume do bạn tạo ra thì sẽ không dính th trên (Xóa container sẽ không xóa volume)

Volume chỉ được mount lúc 'docker run'

docker volume ls
docker volume create [VOLUME_NAME]

Tạo container với database 

Postgres - 5432
    - Từ version 17 trở xuống: `/var/lib/postgresql/data`
    - Từ version 18 trở lên: `/var/lib/postgresql`
MySQL - 3306 - `/var/lib/mysql`
SQLSever - 1433 - `/var/opt/mssql`
Mongo - 27017 - `/data/db`


# PostgreSQL 18
docker volume create postgres_data_volume

# Mount volume
docker run --name database -d -v postgres_data_volume:/var/lib/postgresql -e POSTGRES_PASSWORD=12345 -p 5432:5432 postgres:18

# Bind Mount đường dẫn trên host (recommend)
docker run --name database -d -v ./postgres_data:/var/lib/postgresql -e POSTGRES_PASSWORD=12345 -p 5432:5432 postgres:18 

# Mongo
docker volume create mongo_data_volume
docker run --name mongo -d -v mongo_data_volume:/data/db -p 27017:27017 mongo  

# MySQL
docker volume create mysql_data_volume
docker run --name mysql -d -v mysql_data_volume:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123456 mysql

# SQL Server
docker volume create sqlserver_data_volume
docker run --name sqlserver -d -v sqlserver_data_volume:/var/opt/mssql -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='YourStrong!Pass123' mcr.microsoft.com/mssql/server:2022-latest

# Tạo nháp dữ liệu cho db postgresql
psql -U postgres

# show database 
\l

# show table
\dt 

CREATE TABLE users (name VARCHAR(255));
INSERT INTO users (name) VALUES ('LONG'), ('CHUONG'), ('LAM');

SELECT * FROM users;

# BACKUP VOLUME

Từ volume export ra file 
```bash
docker run --rm -v database_data_volume:/var/lib/postgresql -v "$PWD":/backup ubuntu tar cvf /backup/database_data_volume.tar -C /var/lib/postgresql .
```

`docker run --rm`: Tạo 1 container tạm, tự động xóa container 

[host_path]:[container_path]
`-v database_data_volume:/var/lib/postgresql`: gắn volume database_data_volume vào container
`-v "$PWD":/backup` : lấy đường dẫn ở trên máy (host) và bind mount vào folder bên trong container tạm 
`ubuntu`: dùng image của ubuntu
`tar`: tạo ra file có đuổi .tar
`c`: create
`v`: show các folder được nén 
`f`: tên file output
`x`: giải nén file thành dữ liệu extract
`-C`: giống cd, di chuyển tới folder ta cần lấy dữ liệu  

# RESTORE VOLUME (tar)
```bash
docker run --rm -v database_data_volume:/var/lib/postgresql -v "$PWD":/backup ubuntu tar xvf /backup/database_data_volume.tar -C /var/lib/postgresql
``` 

# Clear - dọn dẹp rác

```bash
docker system df -v
```

`-v`: verbose show chi tiết

Images  
Xóa bỏ image không sài ví dụ: <none><none>
```bash
docker image prune
```
Containers
Không nên sài, tự kiểm soát   
```bash
docker container prune 
```
Local Volumes   
Không sài, dính tới volume dính tới tiền bạc
```bash
docker volume prune
```

Build Cache
```bash
docker builder prune
```    

Combo hay sài
```bash
docker builder prune
docker image prune
``` 

# Tối ưu Dockerfile

## Cache (Theo chiều dọc)
- Tăng tốc độ build

## Stage (giai đoạn) 

