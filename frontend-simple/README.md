# 🚀 Frontend Simples - Student Management

## Frontend ULTRA-RÁPIDO com Vue 3 + Vite + Vuetify

**NÃO MAIS NUXT!** Este é um frontend simples e **MUITO MAIS RÁPIDO**! ⚡

### ⚡ Performance
- **Startup:** 2-5 segundos (vs 30s+ do Nuxt)
- **Hot Reload:** ~0.1-0.5s (quase instantâneo)
- **Build:** Super rápido
- **Sem SSR** = Sem complicação!

## 🚀 Como Usar

### 1. Instalar dependências
```bash
npm install
```

### 2. Rodar em desenvolvimento  
```bash
npm run dev
```

### 3. Acessar
```
http://localhost:5173
```

## 📱 Funcionalidades

### ✅ Telas Implementadas
- **Login/Register** - `/login`
- **Lista de Estudantes** - `/students`
- **CRUD Completo** de estudantes
- **Autenticação JWT**
- **Design responsivo** com Vuetify

### 🔧 Tecnologias
- **Vue 3** - Framework principal
- **Vite** - Build tool ultra-rápido
- **Vuetify** - UI Components
- **Vue Router** - Roteamento
- **Axios** - HTTP client
- **Pinia** - State management (se necessário)

## 🎯 Como Funciona

### 1. **Login/Register**
- Acesse `http://localhost:5173` 
- Redirecionará para `/login`
- Use email e senha para login/registro
- Token JWT salvo no localStorage

### 2. **Gestão de Estudantes**  
- Lista todos os estudantes
- Adicionar novo estudante
- Editar estudante existente
- Excluir estudante
- Validação de autenticação automática

## 🔗 Integração com Backend

**API Base URL:** `http://localhost:5264/api`

### Endpoints usados:
- `POST /auth/login` - Login
- `POST /auth/register` - Registro  
- `GET /student` - Listar estudantes
- `POST /student` - Criar estudante
- `PUT /student/{id}` - Atualizar estudante
- `DELETE /student/{id}` - Excluir estudante

## 🎉 Vantagens vs Nuxt

| Aspecto | Nuxt | Vue+Vite |
|---------|------|----------|
| Startup | 30s+ | 2-5s ⚡ |
| Hot Reload | 1-3s | 0.1-0.5s ⚡ |
| Complexidade | Alta | Baixa ⚡ |
| Bundle Size | Grande | Pequeno ⚡ |
| Configuração | Complexa | Simples ⚡ |

## 🛠️ Scripts Disponíveis

```bash
npm run dev      # Desenvolvimento (porta 5173)
npm run build    # Build para produção
npm run preview  # Preview do build
```

## 🐛 Troubleshooting

### Se não carregar:
1. Certifique-se que a API está rodando (`http://localhost:5264`)
2. Verifique se não há conflito de porta
3. Limpe cache: `rm -rf node_modules/.vite`

### CORS Issues:
- API já configurada para CORS
- Se houver problema, verifique configuração no backend

## 🎯 Próximos Passos

1. ✅ Login/Register funcionando
2. ✅ CRUD de estudantes completo  
3. ✅ Autenticação JWT
4. ✅ Design responsivo
5. 🔄 Validações extras (se necessário)
6. 🔄 Filtros/Busca (se necessário)

**🎉 AGORA SIM: FRONTEND RÁPIDO E FUNCIONAL!** ⚡
