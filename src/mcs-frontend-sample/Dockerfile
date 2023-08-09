FROM node:16 AS build

COPY ["package.json","package.json"]
COPY ["yarn.lock","yarn.lock"]

RUN yarn install

COPY . .
RUN yarn run build

FROM nginx:1.24 AS final
COPY ["nginx.conf", "/etc/nginx/conf.d/default.conf"]
COPY --from=build /dist/ /usr/share/nginx/html/

CMD ["nginx", "-g", "daemon off;"]