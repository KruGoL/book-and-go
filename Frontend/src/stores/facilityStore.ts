import useApi, { useApiRawRequest } from '@/model/api';
import { Facility } from '@/model/facility';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useFacilitiesStore = defineStore('FacilitiesStore', () => {
  let allFacilities: Facility[] = [];
  let Facilities = ref<Facility[]>([]);
  let facility= ref<Facility>();

  //Authorized
  const loadAcc = async () => {
    allFacilities = await loadAccountFacilities();
    Facilities.value = allFacilities;
  };

  const loadAccountFacilities = async () => {
    const apiGetFacilities = useApi<Facility[]>('Accounts/facilities', {
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    await apiGetFacilities.request();

    if (apiGetFacilities.response.value) {
      return apiGetFacilities.response.value!;
    }

    return [];
  };
  const getAccFacilityById = (id: string) => {
    return allFacilities.find((Facility) => Facility.id === id);
  };

  const addFacility = async (Facility: Facility) => {
    const apiAddFacility = useApi<Facility>('facilities', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(Facility),
    });

    await apiAddFacility.request();
    if (apiAddFacility.response.value) {
      allFacilities.push(apiAddFacility.response.value!);
      Facilities.value = allFacilities;
    }
  };

  const updateFacility = async (Facility: Facility) => {
    const apiAddFacility = useApi<Facility>('facilities/' + Facility.id, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(Facility),
    });

    await apiAddFacility.request();
    if (apiAddFacility.response.value) {
        await  loadAcc();
    }
  };

  const deleteFacility = async (Facility: Facility) => {
    const deleteFacilityRequest = useApiRawRequest(`Facilities/${Facility.id}`, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    const res = await deleteFacilityRequest();

    if (res.status === 204) {
      let id = allFacilities.indexOf(Facility);

      if (id !== -1) {
        allFacilities.splice(id, 1);
      }

      id = allFacilities.indexOf(Facility);

      if (id !== -1) {
        allFacilities.splice(id, 1);
      }

      Facilities.value = allFacilities;
    }
  };

  //Allow

  const loadFicilityById = async (id: string) => {
    const url: string = 'Facilities/' + id ;
    const apiGetFacility = useApi<Facility>(url, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
    });

    await apiGetFacility.request();

    if (apiGetFacility.response.value) {
      facility.value = apiGetFacility.response.value!;
      return facility.value;
    }
    return;
  };


  return {
    Facilities,
    facility,
    loadAcc,
    getAccFacilityById,
    addFacility,
    deleteFacility,
    updateFacility,
    loadFicilityById 
  };
});
