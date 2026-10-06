FROM node:24-alpine AS builder

WORKDIR /app

# tải thư viện
COPY package-lock.json .
COPY package.json .
RUN npm install

# build
COPY . .
# build vào folder dist
RUN npm run build

FROM nginx:alpine

# Khong can workdir do la workdir file do co roi la no dung con khong co la no tao moi 
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

# docker build -t js_react_vite_image .
# docker run --name js_react_vite_container -d -p 3001:80 js_react_vite_image  
