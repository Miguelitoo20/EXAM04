#!/bin/bash

docker stop pokemon-container flask-container react-container 2>/dev/null
docker rm pokemon-container flask-container react-container 2>/dev/null

# Crear una red Docker
docker network create pokemon_net 2>/dev/null

# Construir imagenes
docker build -t flask-api ./flask-api
docker build -t pokemon-api ./PokemonApi
docker build -t react-app ./pokemon-frontend

# Correr contenedores
docker run -d --name flask-container --net pokemon_net flask-api
docker run -d --name pokemon-container --net pokemon_net pokemon-api
docker run -d --name react-container --net pokemon_net -p 80:80 react-app

echo "Esperando a que los servicios estén listos..."
sleep 10

# Verificar que los servicios estén respondiendo
#echo "Verificando Flask API..."
#curl -f http://localhost:5000/pokemons || echo "Error: Flask API no responde"

#echo "Verificando C# API..."
#curl -f http://localhost:3000/pokemons || echo "Error: C# API no responde"

#echo "Verificando React App..."
#curl -f http://localhost || echo "Error: React App no responde"