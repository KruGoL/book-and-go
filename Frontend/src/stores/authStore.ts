import useApi from '@/model/api';
import { AuthResponse, User } from '@/model/user';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAuthStore = defineStore('userStore', () => {

  const isAuthenticated = computed(() => Boolean(localStorage.getItem('token')));

  const login = async (loginUser: User): Promise<boolean> => {
    const apiLogin = useApi<AuthResponse>('users/login', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(loginUser),
    });

    await apiLogin.request();

    if (apiLogin.response.value && apiLogin.response.value.token) {
      const token = apiLogin.response.value.token;
      localStorage.setItem('token' , token );
      localStorage.setItem('userName' , loginUser.username );
      console.log(token);

      return true;
    }

    return false;
  };
  const register = async (register: User) => {
    const apiRegister = useApi<AuthResponse>('users/register', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(register),
    });

    await apiRegister.request();
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
  };

  return {
    isAuthenticated,
    login,
    logout,
    register
  };
});
