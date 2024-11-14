import { Account } from '@/model/account';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi from '@/model/api';
import { useAuthStore } from '@/stores/authStore';

export const useAccountStore = defineStore('accountStore', () => {
  const authStore = useAuthStore();
  let allAccounts: Account[] = [];
  let accounts = ref<Account[]>([]);
  let userAcc = ref<Account>();

  const loadAllAccounts = async () => {
    const apiGetaccounts = useApi<Account[]>('accounts', {
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    await apiGetaccounts.request();
    if (apiGetaccounts.response.value) {
      return apiGetaccounts.response.value!;
    }
    return [];
  };
  const loadUserAccount = async () => {
    const apiGetaccount = useApi<Account>('accounts', {
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    await apiGetaccount.request();
    if (apiGetaccount.response.value) {
      userAcc.value = apiGetaccount.response.value!;
      return userAcc.value;
    }
    return;
  };
  const load = async () => {
    allAccounts = await loadAllAccounts();
    accounts.value = allAccounts;
  };
  const getaccountById = (id: string) => {
    const r = allAccounts.find((account) => account.id === id);
    return accounts.value.find((account: Account) => account.id === id);
  };

  const addAccount = async (account: Account) => {
    const apiAddAccount = useApi<Account>('accounts/', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
      body: JSON.stringify(account),
    });
    console.log(apiAddAccount);
    await apiAddAccount.request();
  };

  const updateAccount = async (updated: Account) => {
    const apiAddAccount = await useApi<Account>('accounts/' + updated.id, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(updated),
    });

    await apiAddAccount.request();
    if (apiAddAccount.response.value) {
      return true;
    }
    return false;
  };

  return {
    accounts,
    load,
    getaccountById,
    updateAccount,
    addAccount,
    loadUserAccount,
    userAcc,
  };
});
