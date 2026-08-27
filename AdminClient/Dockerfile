# Dev-mode Dockerfile — runs `vite dev`, matches the 3003:5173 port mapping in docker-compose.yml
FROM node:20-alpine
 
WORKDIR /app
 
COPY package*.json ./
RUN npm install
 
COPY . .
 
EXPOSE 5173
 
# --host 0.0.0.0 is required so Vite's dev server is reachable from outside the container
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]
 