FROM node:20-alpine3.19

WORKDIR /app

COPY package*.json .

RUN yarn install

COPY . .

CMD ["yarn", "run", "dev"]

# RUN apk update && apk add --no-cache \
#     curl \
#     busybox-extras \
#     bind-tools \
#     netcat-openbsd \
#     iputils \
#     net-tools \
#     unzip \
#     && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
#     && unzip awscliv2.zip \
#     && ./aws/install \
#     && rm -rf aws awscliv2.zip
