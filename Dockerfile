FROM ubuntu:26.04

# Core utilities
RUN apt-get update && apt-get install -y \
  curl \
  wget \
  git \
  vim \
  sudo \
  && rm -rf /var/lib/apt/lists/*

# .NET 10
RUN apt-get update && apt-get install -y \
  dotnet-sdk-10.0 \
  && rm -rf /var/lib/apt/lists/*

# Entity Framework Core CLI
RUN dotnet tool install --global dotnet-ef

ENV PATH="/root/.dotnet/tools:${PATH}"

# PostgreSQL 18
RUN apt-get update && apt-get install -y \
  postgresql-18 \
  postgresql-client-18 \
  && rm -rf /var/lib/apt/lists/*

# Node.js 22
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - \
  && apt-get update \
  && apt-get install -y nodejs \
  && rm -rf /var/lib/apt/lists/*

# pnpm
RUN corepack enable \
  && corepack prepare pnpm@latest --activate
ENV CI=true

WORKDIR /app

COPY start.sh /start.sh
RUN chmod +x /start.sh

COPY Backend/Testing/Miscellaneous/busy_week_transactions.sql /app/busy_week_transactions.sql

WORKDIR /app


CMD ["sleep", "infinity"]