FROM node:24-alpine AS builder

WORKDIR /app

# tai thu vien
COPY package-lock.json .
COPY package.json . 
RUN npm install

# build
COPY . . 
# se build vao folder dist 
RUN npm run build 

FROM node:24-alpine 

WORKDIR /app

COPY --from=builder app/dist/ ./dist 
COPY --from=builder app/node_modules ./node_modules

EXPOSE 3000 

CMD ["node", "dist/main"]

# docker build -t js_nestjs_image .
# docker run --name js_nestjs_container -d -p 8004:3000 --env-file=.env js_nestjs_image