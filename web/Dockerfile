# Stage 1: Dependencies
FROM node:20-alpine3.19 AS deps
WORKDIR /app

COPY package*.json ./
COPY yarn.lock ./
RUN yarn install --frozen-lockfile

# Stage 2: Builder
FROM node:20-alpine3.19 AS builder
WORKDIR /app

COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN yarn build

# Stage 3: Runner
FROM node:20-alpine3.19 AS runner
WORKDIR /app

ENV NODE_ENV=production

# Install network tools and AWS CLI
RUN apk update && apk add --no-cache \
    curl \
    busybox-extras \
    bind-tools \
    netcat-openbsd \
    iputils \
    net-tools \
    unzip \
    && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
    && unzip awscliv2.zip \
    && ./aws/install \
    && rm -rf aws awscliv2.zip

# Copy necessary files from builder
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

EXPOSE 3000

CMD ["node", "server.js"]