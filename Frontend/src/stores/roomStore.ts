import { Room } from '@/model/room';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi from '@/model/api';

export const useRoomStore = defineStore('roomStore', () => {
  let allRooms: Room[] = [];
  let rooms = ref<Room[]>([]);

  const loadRooms = async () => {
    const apiGetRooms = useApi<Room[]>(
      'rooms' /*, {
      headers: { Authorization: 'Bearer ' + authStore.token },
    }*/,
    );

    await apiGetRooms.request();
    if (apiGetRooms.response.value) {
      return apiGetRooms.response.value!;
    }
    return [];
  };
  const loadRoomsByFacisilityId = async (id: string) => {
    const url: string = 'Facilities/' + id + '/rooms';
    const apiGetRooms = useApi<Room[]>(url, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
    });

    await apiGetRooms.request();

    if (apiGetRooms.response.value) {
      return apiGetRooms.response.value!;
    }
    return [];
  };

  const load = async (id: string) => {
    allRooms = await loadRoomsByFacisilityId(id);
    rooms.value = allRooms;
  };

  const getRoomById = (id: string) => {
    const r = allRooms.find((room) => room.id === id);
    return rooms.value.find((room: Room) => room.id === id);
  };

  return { rooms, load, getRoomById };
});
