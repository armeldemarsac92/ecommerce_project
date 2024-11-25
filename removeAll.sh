#!/bin/bash

cat << "EOF"
  _               _         _     _                         
 | |   ___  _ __ / | _ __  | |_  | |__   _   _              
/ __) / __|| '__|| || '_ \ | __| | '_ \ | | | |             
\__ \| (__ | |   | || |_) || |_  | |_) || |_| |             
(   / \___||_|   |_|| .__/  \__| |_.__/  \__, |             
 |_|_     ____      |_| _      _   _    _|___/ _   _  _   _ 
|  _ \   / __ \ __   __/ |  __| | | |  / __ \ | | | || \ | |
| | | | / / _` |\ \ / /| | / _` | | | / / _` || | | ||  \| |
| |_| || | (_| | \ V / | || (_| | | || | (_| || |_| || |\  |
|____/  \ \__,_|  \_/  |_| \__,_| | | \ \__,_| \___/ |_| \_|
         \____/                   |_|  \____/               
EOF

docker stop $(docker ps -q)
docker container prune -f
docker image prune -a -f
docker volume rm $(docker volume ls -q)
docker network prune -f
echo 'everything cleaned'
