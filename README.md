# Robô Max: Evolução do Projeto Fritz

O **Robô Max** é uma extensão do projeto open-source original **Fritz**, desenvolvido pela XYZbot. O Fritz passou por mais de 20 versões de aprimoramento, com foco na escalabilidade e usabilidade, sendo projetado para fabricação em massa. Agora, o Robô Max incorpora uma série de modificações que ampliam suas funcionalidades e interatividade, elevando o projeto a uma nova fase de desenvolvimento.

### Projeto Original: [Fritz by XYZbot](https://www.kickstarter.com/projects/1591853389/fritz-a-robotic-puppet)

---

## Modificações e Novas Funcionalidades do Robô Max

O Robô Max traz diversas melhorias significativas em relação ao projeto Fritz, com destaque para conectividade, interatividade e automação. As principais modificações incluem:

1. **Comunicação Cliente-Servidor via Sockets**  
   Foi implementada uma comunicação via **sockets** que permite controlar o Robô Max remotamente a partir de outro computador. Após a conexão entre cliente e servidor ser estabelecida, o Max pode receber comandos para realizar fala e sincronizar os movimentos labiais. O software cliente responsável por esse controle pode ser encontrado na pasta `ChatCliente`.

2. **Randomização de Mensagens e Movimentos**  
   Agora o Max conta com a randomização das falas e dos movimentos labiais. Essa funcionalidade permite que o robô fale textos pré-definidos enquanto realiza gestos de maneira aleatória, criando uma experiência mais dinâmica e realista.

3. **Integração com a API da OpenAI**  
   Uma nova aba foi adicionada ao software, permitindo que o Max se comunique através da API da OpenAI. Para isso, foi desenvolvida uma aplicação em **Python** que se comunica diretamente com o software do Max utilizando a biblioteca **SpeechRecognition**. A sincronização dos movimentos continua sendo feita via **WinForms**, aproveitando a biblioteca **Lip Sync**, que oferece uma solução eficiente para movimentos labiais, algo que ainda não foi replicado de forma satisfatória em Python.

4. **Correção de Bug nos Movimentos Salvos**  
   Um bug relacionado à execução de movimentos salvos na memória do Arduino foi corrigido. Antes, os movimentos estavam sendo executados de maneira invertida ao serem reproduzidos. Agora, o problema foi resolvido, garantindo que os movimentos sejam executados corretamente.

5. **Controle de Dispositivos com ESP8266**  
   Utilizando a biblioteca **SpeechSynthesizer** do .NET Framework, foi implementado um sistema de controle de dispositivos por comandos de voz. Isso permite acionar cargas como luzes conectadas via **ESP8266**, trazendo funcionalidades de automação ao Max.

6. **Sincronização de Áudio e Movimentos**  
   Foi adicionada a capacidade de salvar o áudio gerado pelo sistema de **text-to-speech**. Isso permite criar uma sincronização precisa entre o áudio e os movimentos do robô, tornando as interações mais naturais. Embora o processo ainda precise ser refinado, a base está implementada, permitindo que o Max opere de forma mais independente.

---

Essas modificações transformam o Robô Max em um projeto com maior flexibilidade, permitindo controle remoto, integração com IA, automação residencial e maior realismo nas interações. Embora o desenvolvimento continue, o Max já representa um avanço significativo em relação ao projeto original Fritz, com novas possibilidades de uso.

### Projeto Youtube: [Construção Robot Max](https://www.youtube.com/shorts/sZrK8_OPXAA)
