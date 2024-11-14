import { ChartItem } from '@/types/draggable-panel';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';

export const useBookableObjectStore = defineStore('bookableObjectStore', () => {
  let allBookableObjects: ChartItem[] = [];
  let bObjects = ref<ChartItem[]>([]);

  const loadbookableObjects = async () => {
    const apiGetBookableObjects = useApi<ChartItem[]>(
      'bookableObjects' /*, {
      headers: { Authorization: 'Bearer ' + authStore.token },
    }*/,
    );

    await apiGetBookableObjects.request();
    if (apiGetBookableObjects.response.value) {
      return apiGetBookableObjects.response.value!;
    }
    return [];
  };
  const loadBookableObjectsByRoomId = async (id: string) => {
    const url: string = 'rooms/' + id + '/objects';
    const apiGetBookableObjects = useApi<ChartItem[]>(url, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
    });

    await apiGetBookableObjects.request();

    if (apiGetBookableObjects.response.value) {
      return apiGetBookableObjects.response.value!;
    }
    return [];
  };

  const load = async (id: string) => {
    allBookableObjects = await loadBookableObjectsByRoomId(id);
    bObjects.value = allBookableObjects;
  };

  const getBookableObjectById = (id: string) => {
    const r = allBookableObjects.find((bookableObject) => bookableObject.id === id);
    return bObjects.value.find((bookableObject: ChartItem) => bookableObject.id === id);
  };

  const addBObject = async (object: ChartItem) => {
    console.log(object)
    const apiAddBObject = useApi<ChartItem>('objects/', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(object),
    });
    await apiAddBObject.request();
  };
  const updateBObject = async (object: ChartItem) => {
    object.width = Math.round(object.width!);
    object.height = Math.round(object.height!);
    object.x = Math.round(object.x!);
    object.y = Math.round(object.y!);
    const apiAddAccount = await useApi<object>('objects/' + object.id, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(object),
    });

    await apiAddAccount.request();
    if (apiAddAccount.response.value) {
      return true;
    }
    return false;
  };
  const deleteBObject = async (object: ChartItem) => {
    const deleteBObjectRequest = useApiRawRequest('objects/' + object.id, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    const res = await deleteBObjectRequest();

    if (res.status === 204) {
      await load(object.roomId!);
    }
  };
  return { bObjects, load, getBookableObjectById , addBObject , updateBObject , deleteBObject };
});