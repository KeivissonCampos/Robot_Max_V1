import time
import pyttsx3
import speech_recognition as sr
from Key import API_KEY
import requests
import json
import socket

# Endereço IP e porta do destino
IP_DESTINO = '255.255.255.255'  # Exemplo: Endereço IP do servidor
PORTA_DESTINO = 444      # Exemplo: Porta UDP do servidor

# Criação do socket UDP
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

headers = {"Authorization": f"Bearer {API_KEY}", "Content-type": "application/json"}
link = "https://api.openai.com/v1/chat/completions"
id_model = "gpt-3.5-turbo"
prompt_personagem = "Responda essa pergunta com no máximo 30 palavras se necessario mas pode ser menos, como se você fosse um assistente robô chamado Máx, é engraçado e divertido. Você é uma cabeça robótica utilizado para controlar as coisas de casa, então se ouver solicitações de comando diga que vai executar"+":"

maquina = pyttsx3.init()

def ouvir_microfone():
    x = False
    while True:
        if x:
            time.sleep(15)
            x = False
        else:
            microfone = sr.Recognizer()

        with sr.Microphone() as source:

                microfone.adjust_for_ambient_noise(source)
                print('Diga alguma coisa: ')
                audio = microfone.listen(source)

        try:
            pergunta = microfone.recognize_google(audio, language='pt-BR')

            if "Max" in pergunta:
                print(pergunta)

                if 'lâmpada' in pergunta and 'escritório' in pergunta:
                    comando = "1"
                    try:
                        # Envia o comando para o endereço IP e porta especificados
                        sock.sendto(comando.encode(), (IP_DESTINO, PORTA_DESTINO))
                        print("Comando enviado com sucesso:", comando)
                    except Exception as e:
                        print("Erro ao enviar o comando:", e)
                    finally:
                        # Fecha o socket
                        sock.close()

                body_mensagem = {
                    "model": id_model,
                    "messages": [{"role": "user", "content": prompt_personagem + pergunta}]
                }
                body_mensagem = json.dumps(body_mensagem)

                requesicao = requests.post(link, headers=headers, data=body_mensagem)

                # print(requesicao)
                resposta = requesicao.json()
                mensagem = resposta["choices"][0]["message"]["content"]

                print("resposta: " + mensagem)

                with open('resultado.txt', 'w', encoding='utf-8') as file:
                    file.write(mensagem)
                    x = True

        except sr.UnknownValueError:
            print("repita novamente: ")

ouvir_microfone()
