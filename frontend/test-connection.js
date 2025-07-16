const axios = require('axios');

async function testConnection() {
  const baseUrl = 'http://localhost:5264/api';
  
  console.log('🔍 Testando conexão com:', baseUrl);
  
  try {
    // Teste 1: Verificar se o servidor está rodando
    console.log('\n1️⃣ Testando se o servidor está rodando...');
    const response = await axios.get(`${baseUrl}/student`, {
      timeout: 5000,
      headers: {
        'Authorization': 'Bearer test-token'
      }
    });
    console.log('✅ Servidor está rodando!');
    console.log('Status:', response.status);
    
  } catch (error) {
    console.error('❌ Erro ao conectar:');
    console.error('Status:', error.response?.status);
    console.error('Mensagem:', error.message);
    
    if (error.code === 'ECONNREFUSED') {
      console.error('\n💡 O backend não está rodando!');
      console.error('Execute no terminal:');
      console.error('cd ../backend/StudentManagement.API');
      console.error('dotnet run');
    }
    
    if (error.response?.status === 401) {
      console.error('\n✅ Servidor está rodando, mas precisa de autenticação!');
    }
  }
  
  try {
    // Teste 2: Verificar se o endpoint auth está funcionando
    console.log('\n2️⃣ Testando endpoint de auth...');
    const authResponse = await axios.post(`${baseUrl}/auth/login`, {
      email: 'admin@test.com',
      password: 'admin123'
    }, {
      timeout: 5000
    });
    console.log('✅ Auth endpoint funcionando!');
    console.log('Token:', authResponse.data.token ? 'Recebido' : 'Não recebido');
    
  } catch (error) {
    console.error('❌ Erro no auth endpoint:');
    console.error('Status:', error.response?.status);
    console.error('Mensagem:', error.response?.data || error.message);
  }
}

testConnection(); 