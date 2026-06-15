import {
  useState,
  useRef,
  useEffect,
} from "react";

import ChatMessage from "../../components/chat/ChatMessage";
import TypingIndicator from "../../components/chat/TypingIndicator";
import SuggestedQuestions from "../../components/chat/SuggestedQuestions";

import {
  enviarPregunta,
} from "../../services/chatService";

export default function ChatPage() {
  const [messages, setMessages] =
    useState([
      {
        text:
          "Hola 👋 Soy tu asistente de facturación. ¿En qué puedo ayudarte?",
        isUser: false,
      },
    ]);

  const [input, setInput] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  const bottomRef =
    useRef(null);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({
      behavior: "smooth",
    });
  }, [messages]);

  const sendMessage = async (
    text = input
  ) => {
    if (!text.trim()) return;

    const userMessage = {
      text,
      isUser: true,
    };

    setMessages((prev) => [
      ...prev,
      userMessage,
    ]);

    setInput("");

    try {
      setLoading(true);

      const response =
        await enviarPregunta(text);

      setMessages((prev) => [
        ...prev,
        {
          text: response,
          isUser: false,
        },
      ]);
    } catch {
      setMessages((prev) => [
        ...prev,
        {
          text:
            "Ocurrió un error consultando el asistente.",
          isUser: false,
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="
        flex
        flex-col
        h-[calc(100vh-100px)]
      "
    >
      <div
        className="
          mb-4
          flex
          justify-between
          items-center
        "
      >
        <h1
          className="
            text-2xl
            font-bold
          "
        >
          Asistente IA
        </h1>
      </div>

      <SuggestedQuestions
        onSelect={sendMessage}
      />

      <div
        className="
          flex-1
          overflow-y-auto
          mt-4
          p-4
          bg-slate-50
          dark:bg-slate-900
          rounded-xl
          space-y-3
        "
      >
        {messages.map(
          (msg, index) => (
            <ChatMessage
              key={index}
              text={msg.text}
              isUser={msg.isUser}
            />
          )
        )}

        {loading && (
          <TypingIndicator />
        )}

        <div ref={bottomRef} />
      </div>

      <div
        className="
          mt-4
          flex
          gap-2
        "
      >
        <input
          value={input}
          onChange={(e) =>
            setInput(e.target.value)
          }
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              sendMessage();
            }
          }}
          placeholder="Escribe una pregunta..."
          className="
            flex-1
            border
            rounded-xl
            px-4
            py-3
          "
        />

        <button
          onClick={() =>
            sendMessage()
          }
          disabled={loading}
          className="
            px-5
            rounded-xl
            bg-blue-600
            text-white
          "
        >
          Enviar
        </button>
      </div>
    </div>
  );
}