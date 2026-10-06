#na ná apt install
FROM node:24-alpine AS builder

#similar cd
WORKDIR /app 

#Ưu tiên bắt sự thay đổi của thư viện 
COPY package-lock.json . 
COPY package.json .

#Lệnh cài thư viện / build image
RUN npm install 

COPY . .

#lenh RUN se chay trong luc build image
RUN npm run build

FROM node:24-alpine

WORKDIR /app 

# COPY --from=builder ./app/.next   ./.next  
# COPY --from=builder ./app/node_modules ./node_modules 
# COPY --from=builder ./app/package.json ./package.json 
# COPY --from=builder ./app/public ./public 

#Danh rieng cho source fontend nextjs
COPY --from=builder ./app/.next/standalone/   ./
COPY --from=builder ./app/public   ./public
COPY --from=builder ./app/.next/static   ./.next/static  

EXPOSE 3000

#lenh auto khi start container imag/ chạy container   
CMD ["node", "server.js"]  

# const nextConfig: NextConfig = {
#   /* config options here */
#   output:"standalone"
#   //Keu dev them cai nay neu khong co
# };