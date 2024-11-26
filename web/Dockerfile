FROM oven/bun

WORKDIR ./app/

COPY ./web/bun.lockb .
COPY ./web/package*.json ./app/

RUN bun install

RUN bun add @next/swc-linux-arm64-gnu @next/swc-linux-arm64-musl

COPY ./web /app/

CMD ["bun", "run", "dev"]