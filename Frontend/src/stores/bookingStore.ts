import { Booking } from '@/model/booking';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';
import { date } from 'quasar';

export const useBookingsStore = defineStore('bookingsStore', () => {
  let allBookings: Booking[] = [];
  let bookings = ref<Booking[]>([]);
  let booking= ref<Booking>();

  const loadBookingsByFacisilityId = async (id: string) => {
    const url: string = 'Facilities/' + id + '/bookings';
    const apiGetBookings = useApi<Booking[]>(url, {});

    await apiGetBookings.request();
    console.log('Here');

    if (apiGetBookings.response.value) {
      return apiGetBookings.response.value!;
    }

    return [];
  };

  const load = async (id: string) => {
    allBookings = await loadBookingsByFacisilityId(id);
    bookings.value = allBookings;
  };

  const addBooking = async (booking: Booking, reqMethod: string = 'POST') => {
    console.log(booking);
    const apiAddBooking = useApi<Booking>('bookings/', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(booking),
    });
    await apiAddBooking.request();
  };

  const updateBooking = async (booking: Booking) => {
    const apiAddBooking = useApi<Booking>('bookings/' + booking.id, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(booking),
    });
    await apiAddBooking.request();
  };

  const getBookingById = (id: string) => {
    return bookings.value.find((booking: Booking) => booking.id === id);
  };
  const GETById = async (id: string) => {
    const url: string = 'bookings/' + id ;
    const apiGetBooking = useApi<Booking>(url, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
    });

    await apiGetBooking.request();

    if (apiGetBooking.response.value) {
      booking.value = apiGetBooking.response.value!;
      return booking.value;
    }
    return;
  }
  const filterBookingByDate = (from: string, to: string) => {
    const dFromParse = Date.parse(from);
    const dToParse = Date.parse(to);

    const newDateFrom = new Date(dFromParse);
    const newDateTo = new Date(dToParse);

    let filtred = allBookings.filter(function (book: Booking) {
      const unparsed = date.formatDate(book.bookingDate!, 'HH:mm DD MMMM YYYY');
      const parsed = Date.parse(unparsed);
      const bDate = new Date(parsed);

      return bDate >= newDateFrom;
    });

    filtred = filtred.filter(function (book: Booking) {
      const unparsed = date.formatDate(book.bookingDate!, 'HH:mm DD MMMM YYYY');
      const parsed = Date.parse(unparsed);
      const bDate = new Date(parsed);

      return bDate <= newDateTo;
    });
    return filtred;
  };
  const deleteBooking = async (booking: Booking) => {
    const deleteBookingRequest = useApiRawRequest('bookings/' + booking.id, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
    });

    const res = await deleteBookingRequest();

    if (res.status === 204) {
      await load(booking.facilityId!);
    }
  };
  return {
    bookings,
    booking,
    addBooking,
    updateBooking,
    filterBookingByDate,
    deleteBooking,
    getBookingById,
    load,
    GETById
  };
});
