#!/usr/bin/env bash

ls -la /etc/config
cp /etc/config/*.json /app

ls -la /etc/secret
cp /etc/secret/*.json /app

exec "$@"