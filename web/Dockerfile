FROM oven/bun

WORKDIR /app/

COPY bun.lockb .
COPY package*.json .

RUN bun install

RUN bun add @next/swc-linux-arm64-gnu @next/swc-linux-arm64-musl

RUN apt-get update && apt-get install -y \
    curl \
    telnet \
    dnsutils \
    netcat-openbsd \
    iputils-ping \
    net-tools \
    unzip \
    && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
    && unzip awscliv2.zip \
    && ./aws/install \
    && rm -rf /var/lib/apt/lists/* aws awscliv2.zip

COPY . /app/

CMD ["bun", "run", "dev"]