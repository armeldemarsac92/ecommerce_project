FROM oven/bun

WORKDIR ./app/

COPY bun.lockb .
COPY package*.json .

RUN bun install

RUN bun add @next/swc-linux-arm64-gnu @next/swc-linux-arm64-musl

COPY . /app/

CMD ["bun", "run", "dev"]