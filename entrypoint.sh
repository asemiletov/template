#!/usr/bin/env bash

cp /etc/config/*.json /app
cp /etc/secret/*.json /app

ls /app | grep json

exec "$@"