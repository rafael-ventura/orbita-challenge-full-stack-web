# Nova Arquitetura do Frontend

## Estrutura de Pastas

```
src/
├── components/          # Componentes reutilizáveis
├── services/           # Serviços para comunicação com API
│   ├── authService.ts
│   ├── studentService.ts
│   ├── notificationService.ts
│   ├── api.ts
│   └── index.ts
├── types/              # Definições de tipos TypeScript
│   ├── auth.ts
│   ├── student.ts
│   └── index.ts
├── views/              # Páginas da aplicação
├── utils/              # Funções utilitárias
├── assets/             # Recursos estáticos
└── router/             # Configuração de rotas
```

## Serviços

### AuthService
Responsável por toda comunicação relacionada à autenticação:
- `login(credentials)`: Faz login do usuário
- `register(userData)`: Registra novo usuário
- `logout()`: Faz logout
- `isAuthenticated()`: Verifica se usuário está autenticado
- `getToken()`: Retorna token atual
- `getUser()`: Retorna dados do usuário
- `setAuthData(token, user)`: Salva dados de autenticação

### StudentService
Responsável por toda comunicação relacionada aos estudantes:
- `getAllStudents()`: Busca todos os estudantes
- `getStudentById(id)`: Busca estudante por ID
- `createStudent(studentData)`: Cria novo estudante
- `updateStudent(id, studentData)`: Atualiza estudante
- `deleteStudent(id)`: Remove estudante
- `searchStudents(query)`: Busca estudantes por termo

### NotificationService
Responsável por exibir notificações:
- `success(message)`: Notificação de sucesso
- `error(message)`: Notificação de erro
- `warning(message)`: Notificação de aviso
- `info(message)`: Notificação informativa
- `showError(error)`: Exibe erro de forma padronizada

## Como Usar

### Exemplo de Login
```vue
<script setup>
import { AuthService, NotificationService } from '@/services'

const loginUser = async () => {
  try {
    const response = await AuthService.login(credentials)
    AuthService.setAuthData(response.token, response.user)
    NotificationService.success('Login realizado com sucesso!')
    router.push('/students')
  } catch (error) {
    NotificationService.showError(error)
  }
}
</script>
```

### Exemplo de Listagem de Estudantes
```vue
<script setup>
import { StudentService, NotificationService } from '@/services'
import type { Student } from '@/types'

const students = ref<Student[]>([])

const loadStudents = async () => {
  try {
    students.value = await StudentService.getAllStudents()
  } catch (error) {
    NotificationService.showError(error)
  }
}
</script>
```

## Vantagens da Nova Arquitetura

1. **Simplicidade**: Sem composables complexos ou stores
2. **Reutilização**: Serviços podem ser usados em qualquer componente
3. **Tipagem**: TypeScript forte com interfaces bem definidas
4. **Manutenibilidade**: Código organizado e fácil de manter
5. **Testabilidade**: Serviços isolados são fáceis de testar

## Migração

Para migrar de composables para serviços:

1. Substitua `useAuthApi()` por `AuthService`
2. Substitua `useStudentApi()` por `StudentService`
3. Substitua `useNotification()` por `NotificationService`
4. Remova imports de stores e composables
5. Use as interfaces dos tipos em vez de any 