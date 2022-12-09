
folder_name=$1
template_dir='template'
if [ -d "$folder_name" ];
then
    echo "Folder $folder_name already exists"
    return 1
else
    echo "Copy template as $folder_name"
    cp -r "$template_dir" "$folder_name"
    code "$folder_name"
fi
del $folder
curl 'https://adventofcode.com/2022/day/'$folder_name'/input' \
  -H 'authority: adventofcode.com' \
  -H 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9' \
  -H 'accept-language: en-US,en;q=0.9,pl;q=0.8' \
  -H 'cache-control: max-age=0' \
  -H 'cookie: session=53616c7465645f5f2718b976a6713243b6fde940cccfc5abf30c7b22544c6662710e01d04a4e88a3016766f35d947d9fd6d1a0c0b7772125c013b1a7fd32a8d7' \
  -H 'referer: https://adventofcode.com/2022/day/8' \
  -H 'sec-ch-ua: "Microsoft Edge";v="107", "Chromium";v="107", "Not=A?Brand";v="24"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Linux"' \
  -H 'sec-fetch-dest: document' \
  -H 'sec-fetch-mode: navigate' \
  -H 'sec-fetch-site: same-origin' \
  -H 'sec-fetch-user: ?1' \
  -H 'upgrade-insecure-requests: 1' \
  -H 'user-agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.62' \
  --compressed > $folder_name/input.txt

